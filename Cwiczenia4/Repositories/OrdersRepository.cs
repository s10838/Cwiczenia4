using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Cwiczenia4.Model;

namespace Cwiczenia4.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IConfiguration _configuration;

        public OrdersRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int CreateOrder(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt)
        {
            using (SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;

                    // Validate Amount
                    if (Amount <= 0)
                    {
                        throw new ArgumentException("Amount should be greater than 0.");
                    }

                    // Check if product exists
                    cmd.CommandText = "SELECT IdProduct FROM Product WHERE IdProduct = @IdProduct";
                    cmd.Parameters.AddWithValue("@IdProduct", IdProduct);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            throw new DataException("Product with the given IdProduct does not exist.");
                        }
                    }

                    // Check if warehouse exists
                    cmd.CommandText = "SELECT IdWarehouse FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdWarehouse", IdWarehouse);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            throw new DataException("Warehouse with the given IdWarehouse does not exist.");
                        }
                    }

                    // Check if order exists with required amount
                    cmd.CommandText = "SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdProduct", IdProduct);
                    cmd.Parameters.AddWithValue("@Amount", Amount);
                    cmd.Parameters.AddWithValue("@CreatedAt", CreatedAt);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            throw new DataException("No order exists with the given IdProduct, Amount, and CreatedAt.");
                        }
                    }

                    // Check if order is fulfilled
                    cmd.CommandText = "SELECT 1 FROM Product_Warehouse WHERE IdOrder = (SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt)";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            throw new DataException("Order has already been fulfilled.");
                        }
                    }

                    // Update order to fulfilled
                    cmd.CommandText = "UPDATE [Order] SET FullfilledAt = GETDATE() WHERE IdOrder = (SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt)";
                    cmd.ExecuteNonQuery();

                    // Insert into Product_Warehouse
                    cmd.CommandText = "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@IdWarehouse, @IdProduct, (SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt), @Amount, (SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount, GETDATE()); SELECT SCOPE_IDENTITY();";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@IdWarehouse", IdWarehouse);
                    cmd.Parameters.AddWithValue("@IdProduct", IdProduct);
                    cmd.Parameters.AddWithValue("@Amount", Amount);
                    cmd.Parameters.AddWithValue("@CreatedAt", CreatedAt);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
