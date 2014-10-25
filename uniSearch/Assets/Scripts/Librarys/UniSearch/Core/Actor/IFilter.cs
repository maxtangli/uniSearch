using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// TODO: read about not use generic as base class 
public interface IFilter<T> {
	bool match (T obj, FilterData conditions);
	FilterData Candidates {get;}
}