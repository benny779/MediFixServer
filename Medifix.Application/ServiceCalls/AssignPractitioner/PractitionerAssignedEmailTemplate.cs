using MediFix.Application.Abstractions.Email;

namespace MediFix.Application.ServiceCalls.AssignPractitioner;

public sealed class PractitionerAssignedEmailTemplate : IEmailTemplate
{
    public string GetTemplate() => Template;

    private const string Template = """
                                  @using MediFix.Application.ServiceCalls.AssignPractitioner;
                                  @model PractitionerAssignedEmailModel
                                  
                                  <!DOCTYPE html>
                                  <html lang="en">
                                  <head>
                                      <meta charset="UTF-8">
                                      <meta name="viewport" content="width=device-width, initial-scale=1.0">
                                      <title>New Service Call Assignment</title>
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
                                          .action-required {
                                              background-color: #e74c3c;
                                              color: white;
                                              padding: 10px;
                                              border-radius: 5px;
                                              margin-top: 20px;
                                              font-weight: bold;
                                          }
                                          @@media only screen and (max-width: 480px) {
                                              body {
                                                  padding: 10px;
                                              }
                                          }
                                      </style>
                                  </head>
                                  <body>
                                      <h1>New Service Call Assignment</h1>
                                      
                                      <p>Dear @Model.PractitionerName,</p>
                                      
                                      <p>You have been assigned a new service call. Please review the details below and take appropriate action:</p>
                                      
                                      <div class="details">
                                          <p><strong>Client Name:</strong> @Model.ClientName</p>
                                          <p><strong>Location:</strong> @Model.Location</p>
                                          <p><strong>Type:</strong> @Model.Type</p>
                                          <p><strong>Category:</strong> @Model.Category</p>
                                          <p><strong>Subcategory:</strong> @Model.Subcategory</p>
                                          <p><strong>Priority:</strong> @Model.Priority</p>
                                          <p><strong>Created Date/Time:</strong> @Model.CreatedDateTime</p>
                                          <p><strong>Details:</strong> @Model.Details</p>
                                      </div>
                                      
                                      <div class="action-required">
                                          <p>Action Required:</p>
                                          <ol>
                                              <li>Review the service call details</li>
                                              <li>Contact the client to confirm the appointment</li>
                                              <li>Update the service call status in the system</li>
                                              <li>Prepare any necessary equipment or materials</li>
                                          </ol>
                                      </div>
                                      
                                      <p>If you have any questions or concerns about this assignment, please contact your supervisor immediately.</p>
                                      
                                      <p>Thank you for your prompt attention to this matter.</p>
                                      
                                      <p>Best regards,<br>Service Dispatch Team</p>
                                  </body>
                                  </html>
                                  """;
}
