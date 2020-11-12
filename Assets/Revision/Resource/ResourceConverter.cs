using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceConvereter : ResourceSupply {
	public ResourceConnection[] ResourceIn;

	public override float Request (float Maximum) {
		return ResourceIn.Max ((r) => r.Target.Request(Maximum * r.RequestRatio) / r.RequestRatio);
	}

	public override float Query (float Maximum) {
		return ResourceIn.Max ((r) => r.Target.Query(Maximum) / r.RequestRatio);
	}
}
