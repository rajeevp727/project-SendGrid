# TriSend Azure Infrastructure

Azure infrastructure is defined with Bicep.

## Resources

- App Service Plan
- TriSend API App Service
- TriSend Worker App Service
- Azure SQL Server + TriSendDb
- Azure Service Bus + trisend-messages queue
- Azure Key Vault with managed-identity RBAC
- Azure Storage Account
- Application Insights

The template supports dev, staging and prod.

## Deploy

```bash
az login
az group create --name rg-trisend-dev --location "Central India"

export TRISEND_SQL_ADMIN_PASSWORD='<strong-password>'

az deployment group create \
  --resource-group rg-trisend-dev \
  --template-file infrastructure/main.bicep \
  --parameters infrastructure/main.bicepparam
```

For production, use a separate resource group such as rg-trisend-prod and pass environment=prod.

The SQL administrator password is supplied at deployment time and is not committed to Git.

The API and Worker receive managed identities. Azure RBAC is used for Key Vault and Service Bus access.

For the MVP, public network access is enabled. After the end-to-end system works, add private endpoints, tighter firewall rules, Front Door/WAF and production alerting.
