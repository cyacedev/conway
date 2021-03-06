<p align="center">
<img src="./.github/conway_logo.gif" alt="A glider in Conway's Game of Life">
</p>

# conway

A example Implementation of a Conway's Game of Life simulator,
mostly used to extract statistics.

## Development

Use VS Code with the C# extension from omnisharp.
A simple introduction and setup is explained at [microsoft docs](https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code)

Always use a branch for any changes!

## Usage

`dotnet run input.csv`

### CSV-Format

```csv
FieldSize,ProbabilityForLife,NumberOfIterations,NumberOfSimulations,SaveStatistics,SaveEndState,AverageStats,NameStatisticFile,NameEndStateFile
10,100,1000,10,true,false,true,out/outputfile,out/endstate
```

| Parameter           | Explanation                                                                               |
| :------------------ | :---------------------------------------------------------------------------------------- |
| FieldSize           | The size of the Field in which the starting civilzation will be generated                 |
| ProbabilityForLife  | The Probability in Promille for a cell to be alive at the start                           |
| NumberOfIterations  | Amount of Iterations that a simulation should work trough                                 |
| NumberOfSimulations | Number of Repetitions with the same parameters but new generated cells                    |
| SaveStatistics      | If the programm should save statistics                                                    |
| SaveEndState        | If the programm should save the last state of the field in a simulation                   |
| AverageStats        | If the program should save averages over all simulations of the same type                 |
| NameStatisticFile   | Where the program should store the stats and the name of the files                        |
| NameEndStateFile    | Where the program should store the last state of the simulation and the name of the files |
