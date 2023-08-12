﻿using Ems.Models.Excel;

namespace Ems.Domain.Services;

public interface ILecturerService
{
    Task Import(DateTime requestedAt, List<ExcelLecturerModel> models, CancellationToken token = new());
}