using System;
using System.Collections.Generic;
using Car.Data.DTO;

namespace Car.Data.Service
{
    public interface IGroupsService
    {
      
    	Groups Get(int Id);
        List<Groups> GetList();
        int InsertUpdate(Groups group);
        List<Groups> GetList(int type, string username);
        Groups GetByName(string name, string username);
        int Delete(int Id, string Username);
        
    }
}
