-- TriSend MVP database schema
CREATE TABLE dbo.Tenants (
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Tenants PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Tenants_CreatedAt DEFAULT SYSUTCDATETIME()
);

CREATE TABLE dbo.Messages (
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Messages PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    Channel NVARCHAR(20) NOT NULL,
    Recipient NVARCHAR(320) NOT NULL,
    Body NVARCHAR(MAX) NOT NULL,
    Subject NVARCHAR(500) NULL,
    Status NVARCHAR(30) NOT NULL,
    ProviderMessageId NVARCHAR(200) NULL,
    Error NVARCHAR(2000) NULL,
    CreatedAt DATETIME2 NOT NULL,
    SentAt DATETIME2 NULL,
    CONSTRAINT FK_Messages_Tenants FOREIGN KEY (TenantId) REFERENCES dbo.Tenants(Id),
    CONSTRAINT CK_Messages_Channel CHECK (Channel IN ('sms','whatsapp','email')),
    CONSTRAINT CK_Messages_Status CHECK (Status IN ('queued','processing','sent','delivered','failed'))
);

CREATE INDEX IX_Messages_Tenant_CreatedAt
ON dbo.Messages(TenantId, CreatedAt DESC);

CREATE INDEX IX_Messages_Tenant_Status
ON dbo.Messages(TenantId, Status);

IF NOT EXISTS (
    SELECT 1 FROM dbo.Tenants
    WHERE Id = '00000000-0000-0000-0000-000000000001'
)
BEGIN
    INSERT INTO dbo.Tenants(Id, Name)
    VALUES ('00000000-0000-0000-0000-000000000001', 'TriSend Development Tenant');
END
