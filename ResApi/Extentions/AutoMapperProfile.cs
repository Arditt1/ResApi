﻿using AutoMapper;
using ResApi.DTO;
using ResApi.Models;

namespace ResApi.Extentions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

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