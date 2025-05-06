START TRANSACTION;
ALTER TABLE "KeyVaults" ADD "Expiration" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250407200329_AddKeyVaultExpiration', '9.0.3');

COMMIT;

