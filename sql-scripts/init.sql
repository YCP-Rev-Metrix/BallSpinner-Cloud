IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'revmetrix-r')
BEGIN
    CREATE DATABASE [revmetrix-r];
END

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'revmetrix-u')
BEGIN
    CREATE DATABASE [revmetrix-u];
END

-- Enable remote access
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'remote access', 1;
RECONFIGURE;

-- Disable remote access (optional, if needed)
-- EXEC sp_configure 'remote access', 0;
-- RECONFIGURE;