using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface ISuperAdminHelper
    {
        bool CheckExistingSubjectName(string name);
        bool CreateSubject(SubjectViewModel subjectViewModel);
        List<SubjectViewModel> GetSubjects();
        int GetTotalSubjects();
        int GetTotalStaff();
        int GetTotalStudents();
        SubjectViewModel GetSubjectToEdit(int id);
        bool SaveEditedSubject(SubjectViewModel subjectViewModel);
        bool DeleteSubject(int id);
    }
}
