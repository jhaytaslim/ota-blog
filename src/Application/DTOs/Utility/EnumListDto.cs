using Domain.Enums;
using Domain.Enums.Common;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Utility
{
    public record EnumListDto
    {
        public List<EnumResult> RolesEnumList { get; set; }
        public List<EnumResult> FeatureEnumList { get; set; }
        public List<EnumResult> SectorEnumList { get; set; }
        public List<EnumResult> DependantsEnumList { get; set; }
        public List<EnumResult> ProfileUpdateRequestStatusEnumList { get; set; }
        public List<EnumResult> TokenTypesEnumList { get; set; }
        public List<EnumResult> StatusEnumList { get; set; }
        public List<EnumResult> EmployeeStatusEnumList { get; set; }
        public List<EnumResult> RoleEnum { get; set; }
        public List<EnumResult> TokenTypeEnum { get; set; }
        public List<EnumResult> DependantTypeEnumList { get; set; }
        public List<EnumResult> DependantStatusEnumList { get; set; }
        public List<EnumResult> GenderEnumList { get; set; }
        public List<EnumResult> EmploymentEnumList { get; set; }
    }
}
