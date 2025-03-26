CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "CatalogBrands" (
    "Id" uuid NOT NULL,
    "Brand" text NOT NULL,
    CONSTRAINT "PK_CatalogBrands" PRIMARY KEY ("Id")
);

CREATE TABLE "CatalogTypes" (
    "Id" uuid NOT NULL,
    "Type" text NOT NULL,
    CONSTRAINT "PK_CatalogTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "CatalogItems" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "PictureFileName" text NOT NULL,
    "PictureUri" text NOT NULL,
    "Price" numeric(18,2) NOT NULL,
    "CatalogBrandId" uuid NOT NULL,
    "CatalogTypeId" uuid NOT NULL,
    CONSTRAINT "PK_CatalogItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CatalogItems_CatalogBrands_CatalogBrandId" FOREIGN KEY ("CatalogBrandId") REFERENCES "CatalogBrands" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CatalogItems_CatalogTypes_CatalogTypeId" FOREIGN KEY ("CatalogTypeId") REFERENCES "CatalogTypes" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_CatalogItems_CatalogBrandId" ON "CatalogItems" ("CatalogBrandId");

CREATE INDEX "IX_CatalogItems_CatalogTypeId" ON "CatalogItems" ("CatalogTypeId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250326202035_InitialCreate', '9.0.3');

COMMIT;

