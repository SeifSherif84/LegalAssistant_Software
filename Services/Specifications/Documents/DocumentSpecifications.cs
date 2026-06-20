using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Documents
{
    public class DocumentSpecifications : BaseSpecifications<int, Document>
    {
        private DocumentSpecifications(bool includeLawyer, bool includeCase) : base()
        {
            ApplyInclude(includeLawyer, includeCase);
        }

        // Get all documents for a specific case with conditional includes
        public static DocumentSpecifications GetByCaseId(int caseId, bool includeLawyer, bool includeCase)
        {
            var spec = new DocumentSpecifications(includeLawyer, includeCase);
            spec.ApplyFilterationToGetAllDocumentsForSpecificCase(caseId);
            return spec;
        }


        // Get document by Id with conditional includes
        public static DocumentSpecifications GetById(int documentId, bool includeLawyer, bool includeCase)
        {
            var spec = new DocumentSpecifications(includeLawyer, includeCase);
            spec.ApplyFilterationToGetDocumentWithSpecificId(documentId);
            return spec;
        }


        private void ApplyFilterationToGetAllDocumentsForSpecificCase(int caseId)
        {
            Criteria = D => D.CaseId == caseId;
        }


        private void ApplyFilterationToGetDocumentWithSpecificId(int documentId)
        {
            Criteria = D => D.Id == documentId;
        }


        private void ApplyInclude(bool includeLawyer, bool includeCase)
        {
            if (includeLawyer)
                Includes.Add(D => D.Lawyer);
            if (includeCase) 
                Includes.Add(D => D.Case);
        }

    }
}
