using System.ComponentModel.DataAnnotations.Schema;
using Ems.Core.Entities.Enums;
using Ems.Models.Dtos;

namespace Ems.Domain.Models;

public class StudentRecordsInfoModel
{
    public List<StudentRecordDto> Records { get; set; }
}