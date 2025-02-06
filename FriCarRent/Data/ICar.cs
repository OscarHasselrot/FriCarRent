using FriCarRent.Models;

namespace FriCarRent.Data
{
    public interface ICar
    {
        IEnumerable<Car> GetAll();
        Car GetById(int id);
        void Create(Car car);   
        void Update(Car car);  
        void Delete(Car car);


    }
}
