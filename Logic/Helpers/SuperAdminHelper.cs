using Core.DB;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public class SuperAdminHelper : ISuperAdminHelper
    {
        private readonly AppDbContext _context;

        public SuperAdminHelper(AppDbContext context)
        {
            _context = context;
        }

        public bool CheckExistingSubjectName(string name)
        {
            if (name != null)
            {
                var checkName = _context.Subjects.Where(a => a.Name == name && a.Active).FirstOrDefault();
                if (checkName != null)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CreateSubject(SubjectViewModel subjectViewModel)
        {
            if (subjectViewModel != null)
            {
                var subject = new Subject()
                {
                    Name = subjectViewModel.Name,
                    SubjectDescription = subjectViewModel.SubjectDescription,
                    DateCreated = DateTime.Now,
                    Active = true,
                    Duration = subjectViewModel.Duration,
                };
                _context.Subjects.Add(subject);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<SubjectViewModel> GetSubjects()
        {
            var subjectViewModel = new List<SubjectViewModel>();
                subjectViewModel = _context.Subjects.Where(x => x.Id != 0 && x.Active)
                .Select(x => new SubjectViewModel { 
                    Name = x.Name,
                    SubjectDescription = x.SubjectDescription,
                    DateCreated = x.DateCreated,
                    Id = x.Id,
                    Duration = x.Duration,
                }).ToList();
                return subjectViewModel;
        }

        public int GetTotalStudents()
        {
            return _context.ApplicationUser.Where(a => a.Id != null && !a.Deactivated && a.IsStudent == true).Count();
        }
        public int GetTotalSubjects()
        {
            return _context.Subjects.Where(x => x.Id > 0 && x.Active).Count();
        }
        public int GetTotalStaff()
        {
            return _context.StaffDocuments.Where(a => a.Id > 0 && a.Active).Count();
        }
        public SubjectViewModel GetSubjectToEdit(int id)
        {
            var subjectToEdit = _context.Subjects.Where(a => a.Id == id && a.Active)
                .Select(a => new SubjectViewModel()
                {
                    Name = a.Name,
                    Id = a.Id,
                    SubjectDescription = a.SubjectDescription,
                    DateCreated = a.DateCreated,
                    Duration = a.Duration,
                }).FirstOrDefault();
            if (subjectToEdit != null)
            {
                return subjectToEdit;
            }
            return null;
        }
        public bool SaveEditedSubject(SubjectViewModel subjectViewModel)
        {
            var editSub = _context.Subjects.Where(a => a.Id == subjectViewModel.Id && a.Active).FirstOrDefault();
            if (editSub != null)
            {
                editSub.Name = subjectViewModel.Name;
                editSub.SubjectDescription = subjectViewModel.SubjectDescription;
                editSub.Duration = subjectViewModel.Duration;
                editSub.Active = true;

                _context.Update(editSub);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool DeleteSubject(int id)
        {
            var subToDelete = _context.Subjects.Where(a => a.Id == id && a.Active).FirstOrDefault();
            if (subToDelete != null)
            {
                subToDelete.Active = false;
                _context.Update(subToDelete);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
