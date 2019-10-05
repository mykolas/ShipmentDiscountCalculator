## Requirements
- [dotnet build tools](https://todo)

## Build, run, test
Navigate to project root folder and execute following commands:

- To build project:
    ```
    dotnet build
    ```

- To execute project:
    ```
    dotnet run --porject ShipmentDiscountCalculator
    ```
    or
    ```
    dotnet build
    dotnet run --porject ShipmentDiscountCalculator --no-build
    ```

- To run unit tests:
    ```
    dotnet test ShipmentDiscountCalculatorTests -v n
    ```
    or
    ```
    dotnet build
    dotnet test ShipmentDiscountCalculatorTests -v n --no-build
    ```