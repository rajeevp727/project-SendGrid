export type MessageChannel = "sms" | "whatsapp" | "email";

export interface ProjectSendGridOptions {
  baseUrl: string;
  apiKey: string;
}

export interface SendMessageRequest {
  channel: MessageChannel;
  recipient: string;
  body: string;
  subject?: string;
  idempotencyKey?: string;
}

export interface SendMessageResponse {
  id: string;
  status: string;
}

export class ProjectSendGridClient {
  constructor(private readonly options: ProjectSendGridOptions) {}

  sendSms(recipient: string, body: string, idempotencyKey?: string) {
    return this.send({
      channel: "sms",
      recipient,
      body,
      idempotencyKey,
    });
  }

  sendWhatsApp(recipient: string, body: string, idempotencyKey?: string) {
    return this.send({
      channel: "whatsapp",
      recipient,
      body,
      idempotencyKey,
    });
  }

  sendEmail(
    recipient: string,
    subject: string,
    body: string,
    idempotencyKey?: string,
  ) {
    return this.send({
      channel: "email",
      recipient,
      subject,
      body,
      idempotencyKey,
    });
  }

  private async send(
    request: SendMessageRequest,
  ): Promise<SendMessageResponse> {
    const response = await fetch(
      `${this.options.baseUrl.replace(/\\/$/, "")}/v1/messages`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${this.options.apiKey}`,
        },
        body: JSON.stringify(request),
      },
    );

    if (!response.ok) {
      throw new Error(
        `Project SendGrid request failed: ${response.status}`,
      );
    }

    return response.json();
  }
}
