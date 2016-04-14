Feature: DomainVersion
	Allows to represent a version stamp

@modeling
Scenario Outline: Create new major version stamp
	Given A new major version is created with <major>
	When The version is printed
	Then The printed version looks like "<printed>"
	And The major version looks like "<major>"
	And The minor version looks like ""
	And The revision version looks like ""
	And The build version looks like ""
	Examples: 
	| major | printed |
	| 0     | 0       |
	| 1     | 1       |
	| 9     | 9       |
	| 17    | 17      |

Scenario Outline: Create new major.minor version stamp
	Given A new major.minor version is created with <major> and <minor>
	When The version is printed
	Then The printed version looks like "<printed>"
	And The major version looks like "<major>"
	And The minor version looks like "<minor>"
	And The revision version looks like ""
	And The build version looks like ""
	Examples: 
	| major | minor | printed |
	| 0     | 0     | 0.0     |
	| 0     | 1     | 0.1     |
	| 1     | 0     | 1.0     |
	| 1     | 1     | 1.1     |
	| 9     | 2     | 9.2     |
	| 17    | 3     | 17.3    |

Scenario Outline: Create new major.minor.revision version stamp
	Given A new major.minor.revision version is created with <major>, <minor> and <revision>
	When The version is printed
	Then The printed version looks like "<printed>"
	And The major version looks like "<major>"
	And The minor version looks like "<minor>"
	And The revision version looks like "<revision>"
	And The build version looks like ""
	Examples: 
	| major | minor | revision | printed |
	| 0     | 0     | 2        | 0.0.2   |
	| 0     | 1     | 3        | 0.1.3   |
	| 1     | 0     | 4        | 1.0.4   |
	| 1     | 1     | 5        | 1.1.5   |
	| 9     | 2     | 6        | 9.2.6   |
	| 17    | 3     | 7        | 17.3.7  |

Scenario Outline: Create new major.minor.revision.build version stamp
	Given A new major.minor.revision.build version is created with <major>, <minor>, <revision> and <build>
	When The version is printed
	Then The printed version looks like "<printed>"
	And The major version looks like "<major>"
	And The minor version looks like "<minor>"
	And The revision version looks like "<revision>"
	And The build version looks like "<build>"
	Examples: 
	| major | minor | revision | build | printed     |
	| 0     | 0     | 2        | 9876  | 0.0.2.9876  |
	| 0     | 1     | 3        | 5432  | 0.1.3.5432  |
	| 1     | 0     | 4        | 2468  | 1.0.4.2468  |
	| 1     | 1     | 5        | 12    | 1.1.5.12    |
	| 9     | 2     | 6        | 98765 | 9.2.6.98765 |
	| 17    | 3     | 7        | 0     | 17.3.7.0    |

Scenario Outline: Create new major.minor.details version stamp
	Given A new major.minor.details version is created with <major>, <minor>, "<details1>" and "<details2>"
	When The version is printed
	Then The printed version looks like "<printed>"
	And The major version looks like "<major>"
	And The minor version looks like "<minor>"
	And The revision version looks like "<details1>"
	And The build version looks like "<details2>"
	Examples: 
	| major | minor | details1 | details2 | printed      |
	| 0     | 0     | 2        | alpha1   | 0.0.2.alpha1 |
	| 0     | 1     | 3        | beta2    | 0.1.3.beta2  |
	| 1     | 0     | test     | rc1      | 1.0.test.rc1 |
	| 1     | 1     | 5        | rc2      | 1.1.5.rc2    |
	| 9     | 2     | 6        | 98765    | 9.2.6.98765  |
	| 17    | 3     | 7        | 0        | 17.3.7.0     |

Scenario Outline: Create new details version stamp
	Given A new details version is created with "<details1>", "<details2>" and "<details3>"
	When The version is printed
	Then The printed version looks like "<printed>"
	And The major version looks like "<details1>"
	And The minor version looks like "<details2>"
	And The revision version looks like "<details3>"
	And The build version looks like ""
	Examples: 
	| details1 | details2 | details3 | printed     |
	| 0        | 11       | alpha1   | 0.11.alpha1 |
	| 1        | 3        | beta2    | 1.3.beta2   |
	| 1        | 0        | rc1      | 1.0.rc1     |

Scenario Outline: Try parse a major version
	Given A string "<printed>" is tried to be parsed as a version
	Then The parsing attempt succeed
	And The major version looks like "<major>"
	And The minor version looks like ""
	And The revision version looks like ""
	And The build version looks like ""
	Examples: 
	| printed | major |
	| 1       | 1     |
	| 12      | 12    |

Scenario Outline: Try parse a wrong version
	Given A string "<printed>" is tried to be parsed as a version
	Then The parsing attempt failed
	Examples: 
	| printed |
	| .       |

Scenario Outline: Parse a major version
	Given A string "<printed>" is parsed as a version
	Then The parsing succeed
	And The major version looks like "<major>"
	And The minor version looks like ""
	And The revision version looks like ""
	And The build version looks like ""
	Examples: 
	| printed | major |
	| 1       | 1     |
	| 12      | 12    |

Scenario Outline: Parse a wrong version
	Given A string "<printed>" is parsed as a version
	Then The parsing failed
	Examples: 
	| printed |
	| .       |
