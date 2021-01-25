# conway

A example Implementation of a Conway's Game of Life simulator,
mostly used to extract statistics.

## Development

Use VS Code with the C# extension from omnisharp.
A simple introduction and setup is explained at [microsoft docs](https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code)

Always use a branch for any changes!

## Usage

`dotnet conway.dll input.csv`

## direct run

`dotnet input.csv`

### CSV-Format

```csv
FieldSize,ProbabilityForLife,NumberOfIterations,NumberOfSimulations,SaveStatistics,NameStatisticFile
100,50,1000,1000,true,out/test/testfile
```

| Parameter      | Explanation  |
| :------------- | :---------- |
|  FieldSize | The size of the Field in which the starting civilzation will be generated   |
|  ProbabilityForLife | The Probability in Promille for a cell to be alive at the start |
|  NumberOfIterations | Amount of Iterations that a simulation should work trough |
|  NumberOfSimulations | Number of Repetitions with the same parameters but new generated cells |
|  SaveStatistics | If the programm should save statistics |
|  NameStatisticFile | Where the program should store the stats and the name of the files |
