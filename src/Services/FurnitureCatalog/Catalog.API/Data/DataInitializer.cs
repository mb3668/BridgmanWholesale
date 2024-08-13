using Npgsql;
using Dapper;

namespace Catalog.API.Data;
public class DataInitializer
{
    private readonly string _connectionString;

    public DataInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Initialize database
    public void Initialize()
    {
        using (var DbConnection = new NpgsqlConnection(_connectionString))
        {
            DbConnection.Open();

            // Check if furniture groups exist
            var furnitureGroupTableExists = DbConnection.ExecuteScalar<bool>(
                @"SELECT EXISTS(
                    SELECT 1 FROM information_schema.tables 
                        WHERE table_schema = 'public'
                        and table_name = 'furniture_group'
                );"
            );

            if (!furnitureGroupTableExists)
            {
                DbConnection.Execute(
                    @"CREATE TABLE furniture_group (
                        FurnitureGroupId SERIAL PRIMARY KEY,
                        FurnitureGroupPrice DECIMAL (10, 2) NOT NULL
                    );"
                );
            }

            // Check if set piece exists
            var furnitureSetTableExists = DbConnection.ExecuteScalar<bool>(
                @"SELECT EXISTS(
                    SELECT 1 FROM information_schema.tables 
                        WHERE table_schema = 'public'
                        AND table_name = 'furniture_set'
                );"
            );

            if (!furnitureSetTableExists)
            {
                DbConnection.Execute(
                    @"CREATE TABLE furniture_set (
                        SetPieceId SERIAL PRIMARY KEY,
                        PieceName VARCHAR(100) NOT NULL
                    );"
                );
            }

            // Check if furniture exists
            var furniturePieceTableExists = DbConnection.ExecuteScalar<bool>(
                @"SELECT EXISTS(
                    SELECT 1 FROM information_schema.tables
                        WHERE table_schema = 'public'
                        AND table_name = 'furniture_piece'
                );"
            );

            if (!furniturePieceTableExists)
            {
                DbConnection.Execute(
                    @"CREATE TABLE furniture_piece (
                        FurnitureId SERIAL PRIMARY KEY,
                        FurnitureName VARCHAR(150) NOT NULL,
                        FurnitureImage BYTEA,
                        FurnitureSetPieceId INT REFERENCES furniture_set(SetPieceId),
                        FurniturePriceGroupId INT REFERENCES furniture_group(FurnitureGroupId)
                    );"
                );
            }
        }
    }
}
