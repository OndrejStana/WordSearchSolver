# Word Search Solver

Word Search Solver is a C# command-line application that solves classic word search puzzles. It reads a grid of letters and a list of words from a JSON file, locates the words in the grid (horizontally, vertically, or diagonally, both forwards and backwards), and outputs the string of characters that remain uncrossed.

## How to Run

To run the application, provide the path to your JSON input file as a command-line argument:

```bash
WordSearchSolver.exe path/to/input.json
```

If you're using the .NET CLI, navigate to the `WordSearchSolver` project folder and run the following commands to start the sample app:

```bash
dotnet build
dotnet run input.json
```

### Successfully Solved Puzzle
If the input is valid and all words are found, the application will log the concatenated remaining (uncrossed) characters from the grid, reading from left to right, top to bottom.

Example Output:
```text
info: WordSearchSolver.WordSearchApplication[0]
      Loading file: path/to/input.json
info: WordSearchSolver.WordSearchApplication[0]
      Result: PACIFIK
```

---

## JSON Format Example

The application expects a JSON file containing the `Matrix` (the grid of letters), `Words` (the words to find), and optionally `CrossOnlyFirstOccurence` (a boolean to determine if multiple occurrences of the same word should all be crossed out or just the first one found).

```json
{
  "Matrix": [
    "KALTJSHODA",
    "LLPUKLTOAT",
    "AKTAAKAARR",
    "SAANLAKPEA",
    "ARPOVPTOKK",
    "RHOMOLICEA",
    "KOLSPEKESR",
    "ORAOCAALTP",
    "SPOKVSTIAA",
    "MATKAFTKAT",
    "AIAKOSTKAY"
  ],
  "Words": [
    "ALKA", "HORA", "JUTA", "KAPLE", "KARPATY", "KARTA", "KASA", "KAVKA",
    "KLAS", "KOSMONAUT", "KOST", "KROK", "LAPKA", "MATKA", "OKRASA", "OPAT",
    "OSMA", "PAKT", "PATKA", "PIETA", "POCEL", "POVLAK", "PROHRA", "SEKERA",
    "SHODA", "SOPKA", "TAKT", "TAKTIKA", "TLAK", "VOLHA"
  ],
  "CrossOnlyFirstOccurence": false
}
```

### Expected Result
For the exact JSON provided above, the solver will find all listed words in the matrix and output the remaining letters:
**`PACIFIK`**

---

## Error Handling and Invalid Inputs

The application performs thorough validation to ensure the puzzle is solvable. 

### 1. Validation Errors
If the JSON format is correct but the data violates puzzle constraints, a `Validation failed` error will be logged with the specific reasons, and execution will stop:

* **Missing Matrix/Words**: The matrix and words list cannot be null or empty.
* **Matrix Too Small**: The matrix must have at least 2 rows and each row must have at least 2 columns.
* **Non-Uniform Matrix**: Every string (row) in the `Matrix` must be of the exact same length (it must be a perfect rectangle).
* **Invalid Characters**: Both the `Matrix` and `Words` lists can only contain alphabetical letters. Numeric or special characters will trigger an error.
* **Empty Words**: Words in the list cannot be empty strings.

### 2. File Loading Errors
If the file cannot be accessed or parsed:
* **File Not Found**: An `An error occurred: Could not find file...` message is logged if the file path is incorrect.
* **Invalid JSON Structure**: If the file is not valid JSON, an exception is thrown and logged indicating a deserialization error.

### 3. Solver Errors
During the solving process:
* **Word Not Found**: If a word from the strictly validated list cannot be found anywhere in the matrix (in any of the 8 directions), the solver will continue but log a warning:
  ```text
  warn: WordSearchSolver.Resolver.ResolverService[0]
        Word 'MISSINGWORD' was not found in the matrix.
  ```
