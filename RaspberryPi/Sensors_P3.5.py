import RPi.GPIO as GPIO
import time
import requests

DHTPIN = 17
MOTIONPIN = 27

GPIO.setmode(GPIO.BCM)

MAX_UNCHANGE_COUNT = 100

STATE_INIT_PULL_DOWN = 1
STATE_INIT_PULL_UP = 2
STATE_DATA_FIRST_PULL_DOWN = 3
STATE_DATA_PULL_UP = 4
STATE_DATA_PULL_DOWN = 5

def read_dht11_dat():
	GPIO.setup(DHTPIN, GPIO.OUT)
	GPIO.output(DHTPIN, GPIO.HIGH)
	time.sleep(0.05)
	GPIO.output(DHTPIN, GPIO.LOW)
	time.sleep(0.02)
	GPIO.setup(DHTPIN, GPIO.IN, GPIO.PUD_UP)

	GPIO.setup(MOTIONPIN, GPIO.IN)
        
	unchanged_count = 0
	last = -1
	data = []
	while True:
		current = GPIO.input(DHTPIN)
		data.append(current)
		if last != current:
			unchanged_count = 0
			last = current
		else:
			unchanged_count += 1
			if unchanged_count > MAX_UNCHANGE_COUNT:
				break

	state = STATE_INIT_PULL_DOWN

	lengths = []
	current_length = 0

	for current in data:
		current_length += 1

		if state == STATE_INIT_PULL_DOWN:
			if current == GPIO.LOW:
				state = STATE_INIT_PULL_UP
			else:
				continue
		if state == STATE_INIT_PULL_UP:
			if current == GPIO.HIGH:
				state = STATE_DATA_FIRST_PULL_DOWN
			else:
				continue
		if state == STATE_DATA_FIRST_PULL_DOWN:
			if current == GPIO.LOW:
				state = STATE_DATA_PULL_UP
			else:
				continue
		if state == STATE_DATA_PULL_UP:
			if current == GPIO.HIGH:
				current_length = 0
				state = STATE_DATA_PULL_DOWN
			else:
				continue
		if state == STATE_DATA_PULL_DOWN:
			if current == GPIO.LOW:
				lengths.append(current_length)
				state = STATE_DATA_PULL_UP
			else:
				continue
	if len(lengths) != 40:
		print("Data not good, skip")
		return False

	shortest_pull_up = min(lengths)
	longest_pull_up = max(lengths)
	halfway = (longest_pull_up + shortest_pull_up) / 2
	bits = []
	the_bytes = []
	byte = 0

	for length in lengths:
		bit = 0
		if length > halfway:
			bit = 1
		bits.append(bit)
	print("bits: %s, length: %d" % (bits, len(bits)))
	for i in range(0, len(bits)):
		byte = byte <<  1
		if (bits[i]):
			byte = byte | 1
		else:
			byte = byte | 0
		if ((i + 1) % 8 == 0):
			the_bytes.append(byte)
			byte = 0
	print(the_bytes)
	checksum = (the_bytes[0] + the_bytes[1] + the_bytes[2] + the_bytes[3]) & 0xFF
	if the_bytes[4] != checksum:
		print("Data not good, skip")
		return False

	return the_bytes[0], the_bytes[2]

def main():
	print("Raspberry Pi wiringPi DHT11 Temperature test program\n")
	while True:
		result = read_dht11_dat()
		if result:
			if GPIO.input(MOTIONPIN):
				motion='true'
				print("Motion Detected!")
			else:
				motion='false'
				print("No Motion")

			humidity, temperatureC = result
			temperatureF = '{:04.1f}'.format(((9.0/5.0)*temperatureC)+32)
			print("humidity: %s %%,  Temperature: %s F, Motion: %s" % (humidity, temperatureF, motion))

			roomAddress = 'POR-cr6@ptc.com'
			headers = {'Content-Type': 'application/json'}
			data = {
				'roomAddress': roomAddress,
				'temperature': str(temperatureF),
				'humidity': str(humidity)
                            }
			url = "http://alfred-hack.eastus.cloudapp.azure.com/RestServer/api/rooms/UpdateRoomWeather"
			#url = "http://192.168.1.2/RestServer/api/rooms/UpdateRoomWeather"
			try:
				r = requests.post(url+"/?roomAddress="+roomAddress+"&temperature="+str(temperatureF)+"&humidity="+str(humidity)+"&motion="+str(motion), headers=headers)
				print(r.status_code, r.reason, r.text)
			except:
				print("Unexpected server error")
		time.sleep(10)

def destroy():
	GPIO.cleanup()

if __name__ == '__main__':
	try:
		main()
	except KeyboardInterrupt:
		destroy()
