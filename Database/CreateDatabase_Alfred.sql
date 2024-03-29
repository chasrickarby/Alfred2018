USE [master]
GO

/****** Object:  Database [Alfred]    Script Date: 2/10/2018 7:02:27 PM ******/
CREATE DATABASE [Alfred]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Alfred', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Alfred.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Alfred_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Alfred_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [Alfred] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Alfred].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Alfred] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Alfred] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Alfred] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Alfred] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Alfred] SET ARITHABORT OFF 
GO

ALTER DATABASE [Alfred] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Alfred] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Alfred] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Alfred] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Alfred] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Alfred] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Alfred] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Alfred] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Alfred] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Alfred] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Alfred] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Alfred] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Alfred] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Alfred] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Alfred] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Alfred] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Alfred] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Alfred] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [Alfred] SET  MULTI_USER 
GO

ALTER DATABASE [Alfred] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Alfred] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Alfred] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Alfred] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [Alfred] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Alfred] SET  READ_WRITE 
GO


