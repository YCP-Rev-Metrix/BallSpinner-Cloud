--liquibase formatted sql

--changeset RevMetrix:1
ALTER TABLE [User] ADD test INT;

--rollback ALTER TABLE User DROP COLUMN test;

