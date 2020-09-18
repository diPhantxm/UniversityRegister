using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UniversityRegister.Models;

namespace UniversityRegister.Services
{
    public static class ExcelService
    {
        public static async Task AddStudents(IFormFileCollection excelFiles, string rootPath, UniAPI _api)
        {
            SaveOnServer(excelFiles, rootPath);

            foreach (var file in excelFiles)
            {
                var package = new ExcelPackage(new FileInfo($"{ rootPath }\\{ file.FileName }"));
                var sheets = package.Workbook.Worksheets;

                foreach (var sheet in sheets)
                {
                    for (int i = 1; i < sheet.Dimension.Rows + 1; i++)
                    {
                        var Id = sheet.Cells[i, 1].Value.ToString();
                        var fullName = sheet.Cells[i, 2].Value.ToString().Split(" ");
                        var lastName = fullName[0];
                        var firstName = fullName[1];
                        var middleName = fullName[2];
                        var groupName = sheet.Cells[i, 3].Value.ToString();

                        var groupResponse = await _api.Post<Group, Group>("Groups/Get", new Group() { Name = groupName });

                        Group group = null;
                        if (!(groupResponse.Result is EmptyResult))
                        {
                            group = groupResponse.Value;
                        }
                        else
                        {
                            var groupAdding = await _api.Post<Group, Group>("Groups/Add", new Group(groupName));
                            if (!(groupAdding.Result is EmptyResult))
                            {
                                group = groupAdding.Value;
                            }
                            else continue;
                        }

                        try
                        {
                            var student = new Student(Id, firstName, lastName, middleName, group, new List<Grade>(), new List<StudentsClasses>());
                            await _api.Post<object, Student>("Students", student);
                        }
                        catch (Exception)
                        {
                            
                        }

                    }
                }

                File.Delete(rootPath + file.FileName);
            }

        }
        public static async Task AddTeachers(IFormFileCollection excelFiles, string rootPath, UniAPI _api)
        {
            SaveOnServer(excelFiles, rootPath);

            foreach (var file in excelFiles)
            {
                var package = new ExcelPackage(new FileInfo($"{ rootPath }\\{ file.FileName }"));
                var sheets = package.Workbook.Worksheets;

                foreach (var sheet in sheets)
                {
                    for (int i = 1; i < sheet.Dimension.Rows + 1; i++)
                    {
                        var fullName = sheet.Cells[i, 1].Value.ToString().Split(" ");
                        var lastName = fullName[0];
                        var firstName = fullName[1];
                        var middleName = fullName[2];
                        var password = sheet.Cells[i, 2].Value.ToString();
                        var disciplinesString = sheet.Cells[i, 3].Value.ToString().Split(",");
                        var disciplines = new List<Discipline>();

                        // Add disciplines to db if they don't exist
                        // If exist, add to disciplines list
                        foreach (var discipline in disciplinesString)
                        {
                            var dip = await _api.Post<Discipline, Discipline>($"Disciplines/Get", new Discipline(discipline));
                            if (!(dip.Result is EmptyResult))
                            {
                                disciplines.Add(dip.Value);
                            }
                            else
                            {
                                var addingResponse = await _api.Post<Discipline, Discipline>("Disciplines/Add", new Discipline(discipline));
                                if (addingResponse.Value != null && addingResponse.Value.Id != 0)
                                {
                                    disciplines.Add(addingResponse.Value);
                                }
                            }
                        }

                        // Find teacher. If not found, add
                        var teacher = new TeacherCred(firstName, lastName, middleName, password, new List<TeachersDisciplines>(), new List<Grade>());
                        var teacherResponse = await _api.Post<Teacher, Teacher>("Teachers/Get", teacher);

                        TeacherCred teacherdb = null;
                        if (teacherResponse.Result is EmptyResult)
                        {
                            var teacherAdding = await _api.Post<Teacher, Teacher>("Teachers/Add", teacher);
                            if (!(teacherAdding.Result is EmptyResult))
                            {
                                var teacherAdded = teacherAdding.Value;
                                teacherdb = new TeacherCred
                                {
                                    Id = teacherAdded.Id,
                                    FirstName = teacherAdded.FirstName,
                                    LastName = teacherAdded.LastName,
                                    MiddleName = teacherAdded.MiddleName,
                                    Role = teacherAdded.Role
                                };
                            }
                            else continue;
                        }
                        else
                        {
                            var teacherFind = teacherResponse.Value;
                            teacherdb = new TeacherCred
                            {
                                FirstName = teacherFind.FirstName,
                                LastName = teacherFind.LastName,
                                MiddleName = teacherFind.MiddleName,
                                Id = teacherFind.Id
                            };
                        }

                        var teacherDiscip = new List<TeachersDisciplines>();
                        foreach (var discipline in disciplines)
                        {
                            try
                            {
                                var td = new TeachersDisciplines(teacherdb, discipline);
                                await _api.Post<TeachersDisciplines, TeachersDisciplines>("TeachersDisciplines", td);
                            }
                            catch (Exception)
                            {
                                
                            }

                        }
                    }
                }

                File.Delete(rootPath + file.FileName);
            }
        }
        public static async Task AddGroups(IFormFileCollection excelFiles, string rootPath, UniAPI _api)
        {
            SaveOnServer(excelFiles, rootPath);

            foreach (var file in excelFiles)
            {
                var package = new ExcelPackage(new FileInfo($"{ rootPath }\\{ file.FileName }"));
                var sheets = package.Workbook.Worksheets;

                foreach (var sheet in sheets)
                {
                    for (int i = 1; i < sheet.Dimension.Rows + 1; i++)
                    {
                        var groupName = sheet.Cells[i, 1].Value.ToString();
                        var disciplinesString = sheet.Cells[i, 2].Value.ToString().Split(",");
                        var disciplines = new List<Discipline>();

                        // Add disciplines to db if they don't exist
                        // If exist, add to disciplines list
                        foreach (var discipline in disciplinesString)
                        {
                            var dip = await _api.Post<Discipline, Discipline>($"Disciplines/Get", new Discipline(discipline));
                            if (!(dip.Result is EmptyResult))
                            {
                                disciplines.Add(dip.Value);
                            }
                            else
                            {
                                var addingResponse = await _api.Post<Discipline, Discipline>("Disciplines/Add", new Discipline(discipline));
                                if (addingResponse.Value != null && addingResponse.Value.Id != 0)
                                {
                                    disciplines.Add(addingResponse.Value);
                                }
                            }
                        }

                        // Find group in db. If not found, add
                        var groupResponse = await _api.Post<Group, Group>("Groups/Get", new Group(groupName));
                        Group group = null;
                        if (groupResponse.Result is EmptyResult)
                        {
                            var groupAdded = await _api.Post<Group, Group>("Groups/Add", new Group(groupName));
                            if (!(groupAdded.Result is EmptyResult))
                            {
                                group = groupAdded.Value;
                            }
                        }
                        else
                        {
                            group = groupResponse.Value;
                        }

                        // Add relationship many-to-many
                        foreach (var discipline in disciplines)
                        {
                            try
                            {
                                var groupDisciplines = new GroupsDisciplines(group, discipline);
                                await _api.Post<GroupsDisciplines, GroupsDisciplines>("GroupsDisciplines", groupDisciplines);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }
                }

                File.Delete(rootPath + file.FileName);
            }
        }

        public static void SaveOnServer(IFormFileCollection excelFiles, string path)
        {
            foreach (var file in excelFiles)
            {
                using (var dir = new FileStream($"{ path }\\{ file.FileName }", FileMode.Create))
                {
                    file.CopyTo(dir);
                }
            }
        }
    }
}
