START TRANSACTION;
CREATE TABLE "KeyVaults" (
    "Id" uuid NOT NULL,
    "PrivateKey" text NOT NULL,
    "PublicKey" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_KeyVaults" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250404153025_AddKeyVault', '9.0.3');

COMMIT;

