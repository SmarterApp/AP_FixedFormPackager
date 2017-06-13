# AP_FixedFormPackager

AP_FixedFormPackager
The purpose of the Fixed Form Packager (FFP) is to create valid Administration and Scoring XML packages for the TDS and TIS systems. This tool requires several inputs to accomplish this including three CSV input files and valid credentials to a GitLab instance hosting the item XML files being packaged.

Gitlab

There are four arguments that together provide the FFP the GitLab access it needs to pull down and consume test items:

•	-n A valid username for GitLab (Ex.  developer@fairwaytech.com)
•	-p A valid password for GitLab 
•	-l The GitLab base URL (Ex. https://gitlab-dev.fairwaytech.org/)
•	-g The GitLab group to which your items belong (Ex. ffp-development)*

* The application does not support accessing items under more than one group

CSV

There are three CSV file inputs to the FPP, each serving a different but necessary purpose:

•	-i Item input

This input file contains item-level details about the test packages. It has several columns:
	
•	ItemId	The unique identifier of the item to be included in the generated packages (Ex. 187-1072)
•	FormPartitionId	 The identifier to be assigned to the form partition the item is a part of. This field is required, and if multiple items share a partition their FormPartitionPositions (the following field) must also match. If more than one item share the same partition ID, all items sharing that ID must have identical associated stimuli. Conversely, items without stimuli should be assigned unique partition IDs. This value must be unique in the TDS database (Ex. 187-766)
•	FormPartitionPosition The position of the previously defined segment. Identical partition IDs must have identical form positions to produce valid data (Ex. 1)
•	FormPosition The position of the item within its form and partition. This defines the order in which the items will be presented. (Ex. 2)
•	SegmentId The unique identifier of the segment to which the item belongs. In single-segmented assessments, this value must match the test ID given in the assessment CSV input. In multi-segmented assessments, the opposite is true – this ID in that case must NOT match the test ID. (Ex. (SBAC_PT)SBAC-IRP-Perf-MATH-11-Summer-2015-2016)
•	SegmentPosition The position of the segment in relation to any other segments in the assessment. Like the FormPartitionId and FormPartitionPosition, values in this field must agree with the SegmentId. (ex. 1)
Scoring Information for Items

The following fields are also part of the item input. Each item may have one or more sets of these fields and values for them will be added as separate node sets when the assessment is created. This is done by convention – the grouping of relevant values accomplished by convention. All values in this space are followed by an underscore and then the grouping identifier of the property. For example, MeasurementModel_1 refers to the measurement model for an itemscoredimension that ends up as a sub-node of a testitem in the itempool of both Administration and Scoring packages. If the item requires more than one itemscoredimension, additional sets may be specified by repeating the entire group of column headers below with the next consecutive integer grouping ID (Ex. MeasurementModel_2, ScorePoints_2, … Etc.) If no information is provided in the input file, the application will attempt to generate scoring information by referencing the metadata.xml file for the item.

•	MeasurementModel_1	
•	ScorePoints_1	
•	Dimension_1	
•	Weight_1	
•	a_1	
•	b_1	
•	b0_1	
•	b1_1	
•	b2_1	
•	b3_1	
•	b4_1	
•	c_1

Assessment Information

•	-a This argument specifies the path to the assessment CSV file. This file contains a single row containing information that affects the entire assessment

The following fields appear in the assessment CSV document:

•	UniqueId Note this MUST match the segment outlined in the item input for single-segmented assessments and must NOT match in the case of multi-segmented assessments (Ex. (SBAC_PT)SBAC-IRP-Perf-MATH-11-Summer-2015-2016)
•	Publisher (Ex. SBAC)	
•	Subject	(Ex. ELA)
•	Grade (Ex. 3)
•	AssessmentType (Ex. interim)
•	AssessmentSubtype (Ex. ICA)

These represent the cutpoints between various performance levels and the barrier values for scoring packages beginning the lowest value and defining the barriers between performance levels until the maximum is reached:

•	ScaledLo	
•	ScaledPartition1	
•	ScaledPartition2	
•	ScaledPartition3	
•	ScaledHi

Scoring Information

The computation rules that apply to scoring packages are defined in this CSV file. Similar to the item scoring information above, these parameters may be repeated and are grouped and defined by convention. The “parent” grouping is defined first, followed by any “child” groupings where necessary for the various parameters. For example, ParameterType_1 may have child elements ParameterPropertyName_1_1 and ParameterPropertyName_1_2 as child elements where the first group designation matches the parent.

These values apply to all repeating fields that follow, and are therefore not numbered:

•	Name	(Ex. SBACAttemptedness)
•	ComputationOrder (Ex. 10)
•	BpElementId (Ex. SBAC-1)

These fields are parent fields:

•	ParameterType_1 (Ex. int)
•	ParameterName_1 (Ex. testPart)

These fields are child fields:

•	ParameterPropertyName_1_1	(Ex. indextype)
•	ParameterPropertyValue_1_1	(Ex. string)
•	ParameterComputationRuleIndex_1_1	(Ex. (SBAC)TestAssessment-3M-2016-2017)
•	ParameterComputationRuleValue_1_1	(Ex. 1)
