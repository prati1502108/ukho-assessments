//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Batch_Manager.Models;
//using Attribute = Batch_Manager.Models.Attribute;
//using File = Batch_Manager.Models.File;

//namespace Batch_Manager.Tests.Fake
//{
//    public static class BatchFake
//    {
//        public static Batch GetFakeBatch()
//        {
//            Batch batch = new Batch();
//            batch.BatchId = "8065e72d-97ca-4d94-b02d-678d6c274fa5";

//            batch.BusinessUnit = "BU2";
//            batch.Status = "InProgress";
//            batch.BatchPublishedDate = DateTime.Now.ToString();
//            batch.ExpiryDate = DateTime.Now.ToString();
//            Acl objAcl = new Acl();
//            List<String> strList = new List<String>();
//            strList.Add("User1");
//            strList.Add("User2");
//            objAcl.ReadUsers = strList;
//            List<String> strListGrp = new List<String>();
//            strListGrp.Add("Grp1");
//            strListGrp.Add("Grp2");
//            batch.Acl = objAcl; 
//            List<Attribute> attrList = new List<Attribute>();
//            Attribute objAttr = new Attribute();
//            objAttr.Key = "Key1";
//            objAttr.Value = "Value1";
//            attrList.Add(objAttr);
//            batch.Attributes = attrList;

//            File oFile = new File();
//            List<File> files = new List<File>();
//            oFile.FileName = "File1";
//            oFile.FileSize = "200MB";
//            oFile.Hash = "SHA";
//            ICollection<FileAttribute> attribute = new List<FileAttribute>();
//            FileAttribute fileAttr = new FileAttribute();
//            fileAttr.Key = "Key1";
//            fileAttr.Value = "Value1";
//            attribute.Add(fileAttr);
//            oFile.Attributes = attribute;
//            files.Add(oFile);
//            batch.Files = files;
//            return batch;
//        }
//        public static ErrorDetails GetErrorInfo()
//        {
//            ErrorDetails errDesc = new ErrorDetails();
//            List<Error> errList = new List<Error>();
//            Error err = new Error();
//            errDesc.CorrelationId = "8065e72d-97ca-4d94-b02d-678d6c";
//            err.Source = "Batch Controller";
//            err.Description = "No Record Found.";
//            errList.Add(err);
//            errDesc.Errors = errList;
//            return errDesc; 
//        }

//        public static ErrorDetails GetBatchObjectErrorInfo()
//        {
//            ErrorDetails errDesc = new ErrorDetails();
//            List<Error> errList = new List<Error>();
//            Error err = new Error();
//            errDesc.CorrelationId = "6bb112bd-663d-449f-9716-cb76f2d142b9";
//            err.Source = "Batch Controller";
//            err.Description = "Batch object is null.";
//            errList.Add(err);
//            errDesc.Errors = errList;
//            return errDesc;
//        }
//    }


//}
