using Batch_Manager.DataTableObjects;
using Batch_Manager.Models;

namespace Batch_Manager.Tests.Fake
{
    public static class BatchFake
    {
        public static Batch GetFakeBatch()
        {
            Batch batch = new Batch();
            batch.BatchId = "8065e72d-97ca-4d94-b02d-678d6c274fa5";
            batch.BusinessUnit = "BU2";
            batch.Status = "InProgress";
            batch.BatchPublishedDate = DateTime.Now.ToString();
            batch.ExpiryDate = DateTime.Now.ToString();
            Acl acl = new Acl();
            List<User> users = new List<User>();
            users.Add(new User() { Name = "User1" });
            users.Add(new User() { Name = "User2" });
            acl.ReadUsers = users;
            List<Group> groups = new List<Group>();
            groups.Add(new Group() { Name = "Group1" });
            groups.Add(new Group() { Name = "Group2" });
            acl.ReadGroups = groups;
            batch.Acl = acl;
            List<BatchAttribute> batchAttributes = new List<BatchAttribute>();
            BatchAttribute batchAttribute = new BatchAttribute();
            batchAttribute.Key = "Key1";
            batchAttribute.Value = "Value1";
            batchAttributes.Add(batchAttribute);
            batch.Attributes = batchAttributes;

            BatchFileDto file = new BatchFileDto();
            List<BatchFileDto> files = new List<BatchFileDto>();
            file.FileName = "File1";
            file.FileSize = "200MB";
            ICollection<FileAttribute> fileAttributes = new List<FileAttribute>();
            FileAttribute fileAttribute = new FileAttribute();
            fileAttribute.Key = "Key1";
            fileAttribute.Value = "Value1";
            fileAttributes.Add(fileAttribute);
            file.Attributes = fileAttributes;
            files.Add(file);
            batch.Files = files;
            return batch;
        }
        public static ErrorDetails GetBatchRecordsErrorDetails()
        {
            ErrorDetails errorDetails = new ErrorDetails();
            List<Error> errors = new List<Error>();
            Error error = new Error();
            errorDetails.CorrelationId = "8065e72d-97ca-4d94-b02d-678d6c";
            error.Source = "Batch Controller";
            error.Description = "No Record Found.";
            errors.Add(error);
            errorDetails.Errors = errors;
            return errorDetails;
        }

        public static ErrorDetails GetBatchObjectErrorDetails()
        {
            ErrorDetails errorDetails = new ErrorDetails();
            List<Error> errors = new List<Error>();
            Error error = new Error();
            errorDetails.CorrelationId = "6bb112bd-663d-449f-9716-cb76f2d142b9";
            error.Source = "Batch Controller";
            error.Description = "Batch object is null.";
            errors.Add(error);
            errorDetails.Errors = errors;
            return errorDetails;
        }
    }
}
