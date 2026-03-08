NmmEnvironment - NMM Environmental Sensor Data Viewer
=====================================================

A standalone command line tool to extract the environmental data of measurement files produced by a [SIOS](https://sios-de.com) NMM. The signals are automatically recorded in the respective scan and 3D files.

## Overview
The Nanopositioning and Nanomeasuring Machine (NMM) by [SIOS GmbH](https://sios-de.com) is a versatile metrological instrument. It can perform various dimensional measurements like profile and surface scans with different probing sensors. This tool allows to extract the environmental data (air and sample temperature, humidity, barometric pressure) into a simple CSV file.

## Command Line Usage:  
```
NmmEnvironment inputfile [options]
```

## Options:  
`--pretty (-p)` : Pretty (sort of) CSV file.  
`--noplot` : Do not generate plot.

### Output
A simple CSV-file and a png image (if not suppressed) is produced. The image shows a time plot of the sample and air temperature. 

![Time vs temperature plot](/TestData/T22-0263_L2_15_THP.png)  
Time vs temperature plot of a test measurement including two consecutive scans.  

### Caveat
You must manually close the plot window in order to quit the program.

### Installation
If you do not want to build the application from the source code you can use the released binaries. Just copy the .exe and the .dll files to a directory of your choice. This direcory should be included in the user's PATH variable.

## Dependencies and Acknowledgments
* [At.Matus.IO.NmmReader](https://github.com/matusm/At.Matus.IO.NmmReader)  
* [At.Matus.StatisticPod](https://github.com/matusm/At.Matus.StatisticPod)
* [CommandLineParser](https://github.com/commandlineparser/commandline)
* [Math.NET numerics](https://numerics.mathdotnet.com)

---

**Note**: This library is not officially affiliated with or endorsed by SIOS Meßtechnik GmbH.
