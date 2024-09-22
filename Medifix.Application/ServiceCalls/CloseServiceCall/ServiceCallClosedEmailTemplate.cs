using MediFix.Application.Abstractions.Email;

namespace MediFix.Application.ServiceCalls.CloseServiceCall;

public sealed class ServiceCallClosedEmailTemplate : IEmailTemplate
{
    public const string Subject = "Your Service Call Has Been Closed";

    public string GetTemplate() => Template;

    private const string Template = """
                                    @using MediFix.Application.ServiceCalls.CloseServiceCall;
                                    @model ServiceCallClosedEmailModel
                                    
                                    <!DOCTYPE html>
                                    <html lang="en">
                                    <head>
                                        <meta charset="UTF-8">
                                        <meta name="viewport" content="width=device-width, initial-scale=1.0">
                                        <title>Service Call Closed</title>
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
                                        <h1>Your Service Call Has Been Closed</h1>
                                        
                                        <p>Dear @Model.ClientName,</p>
                                        
                                        <p>We're writing to confirm that your service call has been successfully completed and closed. Here are the final details:</p>
                                        
                                        <div class="details">
                                            <p><strong>Location:</strong> @Model.Location</p>
                                            <p><strong>Type:</strong> @Model.Type</p>
                                            <p><strong>Category:</strong> @Model.Category</p>
                                            <p><strong>Subcategory:</strong> @Model.Subcategory</p>
                                            <p><strong>Practitioner:</strong> @Model.PractitionerName</p>
                                            <p><strong>Completion Date:</strong> @Model.CompletionDate</p>
                                            <p><strong>Close details:</strong> @Model.CloseDetails</p>
                                        </div>
                                        
                                        <p>We hope that the service provided met your expectations. If you have any questions about the completed work or need any further assistance, please don't hesitate to contact us.</p>
                                        
                                        <p>Thank you for choosing our services. We appreciate your business and look forward to serving you in the future.</p>
                                        
                                        <p>Best regards,<br>Your Service Team</p>
                                    </body>
                                    </html>
                                    """;
}
