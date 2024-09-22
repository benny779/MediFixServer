using MediFix.Application.Abstractions.Email;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

public sealed class ServiceCallCreatedEmailTemplate : IEmailTemplate
{
    public const string Subject = "Service Call Confirmation";
    public string GetTemplate() => Template;

    private const string Template = """
                                    @using MediFix.Application.ServiceCalls.CreateServiceCall;
                                    @model ServiceCallCreatedEmailModel

                                    <!DOCTYPE html>
                                    <html lang="en">
                                    <head>
                                        <meta charset="UTF-8">
                                        <meta name="viewport" content="width=device-width, initial-scale=1.0">
                                        <title>Service Call Confirmation</title>
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
                                        <h1>Your Service Call Was Created Successfully</h1>
                                        
                                        <p>Dear @Model.FullName,</p>
                                        
                                        <p>We're pleased to confirm that your service call has been successfully created. Here are the details:</p>
                                        
                                        <div class="details">
                                            <p><strong>Location:</strong> @Model.Location</p>
                                            <p><strong>Type:</strong> @Model.Type</p>
                                            <p><strong>Category:</strong> @Model.Category</p>
                                            <p><strong>Subcategory:</strong> @Model.Subcategory</p>
                                            <p><strong>Priority:</strong> @Model.Priority</p>
                                            <p><strong>Details:</strong> @Model.Details</p>
                                        </div>
                                        
                                        <p>If you need to make any changes or have any questions, please don't hesitate to contact us.</p>
                                        
                                        <p>Thank you for choosing our services.</p>
                                        
                                        <p>Best regards,<br>Your Service Team</p>
                                    </body>
                                    </html>
                                    """;
}
