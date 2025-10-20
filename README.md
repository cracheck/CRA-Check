# CRA-Check

[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![Syft](https://img.shields.io/badge/grype-anchore.io%2Fsyft-orange)](https://github.com/anchore/syft)
[![Grype](https://img.shields.io/badge/grype-anchore.io%2Fgrype-orange)](https://github.com/anchore/grype)

## Overview

**CRA-Check** is a minimal C# application to manage and follow vulnerabilities for softwares. It is based on Syft (SBOm generator) and Grype (Vulnerabilities scanner). The goal is to have a simple tool for free and that work local (not server based)

Target user are the Small and Medium company.

---

## Features

- ✅ Create a SBOMs using the Syft CLI  
- ✅ Scan for vulnerabilities a SBOMs using the Grype CLI
- ✅ Manage software and release
- ⚙️ Compatible with .NET 8  
- 🪟 Windows support (tested)  

---

## Project Status
**MVP** - The wrapper currently provides basic scan for vulnerabilities.
Futur improvements may include:
- Auto scan and raise notification in case of vulnerabilities
- Review the design
- Multi language
- Add other functionalities linked with the Cyber Resilience Act

---

## License
This project is licenses under the Apache 2.0 License
**Note:** Grype itself is licensed under the Apache 2.0 License.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Syft CLI](https://github.com/anchore/syft) 
- [Grype CLI](https://github.com/anchore/grype)
