# MVP Smoke Test

## 1. Start API

```bash
dotnet run --project backend/Communication.Api/Communication.Api.csproj
```

## 2. Health

```bash
curl http://localhost:5000/health
```

Expected: HTTP 200.

## 3. Send test message

```bash
curl -X POST http://localhost:5000/v1/messages \
  -H "Authorization: Bearer <Mvp__ApiKey>" \
  -H "Content-Type: application/json" \
  -d '{"channel":"email","recipient":"test@example.com","subject":"MVP test","body":"Hello from Project SendGrid"}'
```

Expected: HTTP 202 and a queued message ID.

Real provider delivery is enabled only after provider credentials and worker/queue integration are configured.
