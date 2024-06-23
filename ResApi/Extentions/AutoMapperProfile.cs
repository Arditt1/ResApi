using AutoMapper;
using ResApi.DTO;
using ResApi.DTO.OrderDetail;
using ResApi.DTO.Tables;
using ResApi.DTO.TableWaiter;
using ResApi.Models;

namespace ResApi.Extentions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Role, EmployeeDTO>().ReverseMap();
            CreateMap<TableWaiter, EmployeeDTO>().ReverseMap();
            CreateMap<MenuItem, MenuItemDTO>().ReverseMap();
            CreateMap<CategoryMenu, MenuItemDTO>().ReverseMap();
            CreateMap<CategoryMenu, CategoryMenuDTO>().ReverseMap();


            CreateMap<CategoryMenu, MenuItemDTO>().ReverseMap()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<OrderDetail, GetAllOrderDetailsDTO>().ReverseMap();
            CreateMap<Order, GetAllOrderDetailsDTO>().ReverseMap();
            CreateMap<CategoryMenu, GetAllOrderDetailsDTO>().ReverseMap();
            CreateMap<Table, GetAllOrderDetailsDTO>().ReverseMap();
            CreateMap<Employee, GetAllOrderDetailsDTO>().ReverseMap();
            CreateMap<Table, TableDTO>().ReverseMap();
            CreateMap<TableWaiter, TableWaiterDTO>().ReverseMap();
            CreateMap<TableDTO, TableWaiter>().ReverseMap()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Table.Id))
                    .ForMember(dest => dest.TableNumber, opt => opt.MapFrom(src => src.Table.TableNumber));
        }

        public static Employee MapForUpdate(EmployeeDTO empDTO)
        {
            Employee em = new Employee();
            em.Username = empDTO.Username;
            em.Name = empDTO.Name;
            em.RoleId = empDTO.RoleId;
            em.Status = empDTO.Status;
            em.Surname = empDTO.Surname;
            return em;
        }

        public static CategoryMenu MapForRegisterCategory(CategoryMenuDTO catDTO)
        {
            CategoryMenu ca = new CategoryMenu();
            ca.CategoryName = catDTO.CategoryName;
            ca.Photo = catDTO.Photo;
            return ca;
        }

        public static Employee Kamarierat(EmployeeDTO emp)
        {
            Employee employee = new Employee();
            employee.Name = emp.Name;
            employee.Surname = emp.Surname;
            employee.Username = emp.Username;
            employee.Status = emp.Status;
            employee.ContactInfo = emp.ContactInfo;
            return employee;
        }

        public static MenuItem MapForRegisterMenu(MenuItemDTO menuDTO)
        {
            MenuItem manitem = new MenuItem();
            manitem.Name = menuDTO.Name;
            manitem.Price = menuDTO.Price;
            manitem.Description = menuDTO.Description;
            return manitem;
        }
    }
}