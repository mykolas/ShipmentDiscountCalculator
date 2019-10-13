## Requirements
- [.NET Core 3.0 SDK](https://dotnet.microsoft.com/download)

## Run and unit tests
Navigate to project root folder and execute following commands:

- To execute project (if input file is not provided, default name 'input.txt' is assumed):
    ```
    dotnet run --project ShipmentDiscountCalculator input.txt
    ```

- To run unit tests:
    ```
    dotnet test ShipmentDiscountCalculatorTests -v n
    ```

- Commands described above will build project and it will be visible in output. To have clean output build can be done in advance.
    ```
    dotnet build
    dotnet run --project ShipmentDiscountCalculator --no-build input.txt
    dotnet test ShipmentDiscountCalculatorTests -v n --no-build
    ```