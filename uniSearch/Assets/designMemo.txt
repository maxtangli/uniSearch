# Questions

- a common process of OO design
- a common process of OO design validation&implemention

# Reminds

Design core issues before coding
Think about the priority of each part when coding
Stop and think carefully when coding is not fluently

# Design 2h (however, more than 150 hours of failed design/redesign/coding/recoding before)

## UseCase: onClick

UIFilter.get list<OptionGroup>
 list<UIOptionGroup>. each 
  UIOptionGroup.get OptionGroup

filteredData = model.fetch list<OptionGroup>
 allData = remote/resource read
 filteredData = filterChain.filt list<OptionGroup>
  assert valid list
  map text form into key form
  each keyForm OptionGroup 
   find handable filter: where memberLocalFitler.title == OptionGroup.title
   result = filter.filt result

LocalFilter.filt dataList
OneMemberFilter
 title -> getOneMember dataObj,title
 options -> getTargetValues options
 result = dataList.where getOneMember in getTargetValues
 
OneMemberToStringFilter // string,int,enum,bool
 title -> dataObj.getMember(title).toString
 options -> options

OneComparableMemberInRangeFilter // int -1400 1401-2400 2401-2500 2500-
 title -> dataObj.getMember(title), assert comparable
 options -> Ranges
 
## UseCase: init UIFilter

model.list<OptionGroup> filterOptionGroups
UIFilter.set list<OptionGroup>
 each UIOptionGroup.optionGroup = optionGroup
 
how to map optionGroup to UIOptionGroup?

- drag into inspector, onInspector
- if not setted, use stubOptionGroup_title

UIOptionGroup.optionGroup = optionGroup
 set title view if have
 set options list (if it changes)
 if this is single select optionGroup, change select state of options, log warning
 set select state
 
reference issue? @TODO
 
# Coding

## Priority

With enough separation, either part first is OK, with a stub of other part.
However, not sure part first reduce the risk.

Thus
1. OptionGroup
2. UIFilter, test with stubFilter
3. UIOptionGroup, test with a custom sub class
4. FilterModel, design about data types before coding

## OptionGroup 

declare fields(interface). use IDE to generate constructor. 1min

## UIFilter

### 

declare interface. map issues. first flow then detail. 0.5h

### 

Mapping from optionGroup data to ui?
what matter is overall flow, such as the timing of use load members.

1. first time optionGroups.set, where none of ui specified.
manual locate. auto locate: by create order, by name, by optionGroup.title

2. second time optionGroups.set
necessary? 

rough version seems not bad. 0.5h

### 

how to test?

# TODO

- [x] github init
- [x] overall design
- [x] custom editor: later do since too complex
- [x] UIFilterGroups implemention
- [ ] a sample of UIFilterGroup view
- [ ] design of Model
- [ ] implement Filter
- [ ] controller

- [ ] design of Sort
- [ ] implement Sort
- [ ] design of Pager
- [ ] implement Pager
- [ ] organize code

- [ ] github doc

- [x] upload ruby codes
- [x] upload reading list
- [x] refine resume
- [ ] go