using MediFix.Application.Abstractions.Email;

namespace MediFix.Application.ServiceCalls.CancelServiceCall;

public sealed class ServiceCallCancelledEmailTemplate : IEmailTemplate
{
    public const string Subject = "Your Service Call Has Been Cancelled";
    public string GetTemplate() => Template;

    private const string Template = """
                                    @using MediFix.Application.ServiceCalls.CancelServiceCall;
                                    @model ServiceCallCancelledEmailModel
                                    
                                    <!DOCTYPE html>
                                    <html lang="en">
                                    <head>
                                        <meta charset="UTF-8">
                                        <meta name="viewport" content="width=device-width, initial-scale=1.0">
                                        <title>Service Call Cancelled</title>
                                        <style>
                                            body {
                                                font-family: Arial, sans-serif;
                                                line-height: 1.6;
                                                color: #333;
                                                max-width: 600px;
                                                margin: 0 auto;
                                                padding: 20px;
                                            }
                                            h1 {
                                                color: #2c3e50;
                                            }
                                            .details {
                                                background-color: #f9f9f9;
                                                border: 1px solid #ddd;
                                                border-radius: 5px;
                                                padding: 15px;
                                                margin-top: 20px;
                                            }
                                            .details p {
                                                margin: 5px 0;
                                            }
                                            @@media only screen and (max-width: 480px) {
                                                body {
                                                    padding: 10px;
                                                }
                                            }
                                        </style>
                                    </head>
                                    <body>
                                        <h1>Your Service Call Has Been Cancelled</h1>
                                        
                                        <p>Dear @Model.ClientName,</p>
                                        
                                        <p>We're writing to confirm that your service call has been cancelled.
                                        Here are the details of the cancelled service call:</p>
                                        
                                        <div class="details">
                                            <p><strong>Location:</strong> @Model.Location</p>
                                            <p><strong>Type:</strong> @Model.Type</p>
                                            <p><strong>Category:</strong> @Model.Category</p>
                                            <p><strong>Subcategory:</strong> @Model.Subcategory</p>
                                            <p><strong>Cancellation Date:</strong> @Model.CancellationDate</p>
                                            <!--p><strong>Reason for Cancellation:</strong> Model.CancellationReason</p-->
                                        </div>
                                        
                                        <p>If you did not request this cancellation or if you have any questions, please contact us immediately.</p>
                                        
                                        <p>If you need to schedule a new service call in the future, please don't hesitate to reach out to us.</p>
                                        
                                        <p>Thank you for your understanding.</p>
                                        
                                        <p>Best regards,<br>Your Service Team</p>
                                    </body>
                                    </html>
                                    """;
}
