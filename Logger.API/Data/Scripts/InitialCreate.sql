CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Messages" (
    "Id" uuid NOT NULL,
    "Service" text NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    "EventType" text NOT NULL,
    "Status" text NOT NULL,
    "Details" text NOT NULL,
    CONSTRAINT "PK_Messages" PRIMARY KEY ("Id")
);

CREATE TABLE "Consumers" (
    "Id" uuid NOT NULL,
    "LogMessageId" uuid NOT NULL,
    "ConsumerService" text NOT NULL,
    "ConsumedTime" timestamp with time zone NOT NULL,
    "Details" text,
    CONSTRAINT "PK_Consumers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Consumers_Messages_LogMessageId" FOREIGN KEY ("LogMessageId") REFERENCES "Messages" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Consumers_LogMessageId" ON "Consumers" ("LogMessageId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250422205110_InitialCreate', '9.0.3');

COMMIT;

