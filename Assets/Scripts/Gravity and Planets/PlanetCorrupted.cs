using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlanetSpawnerManager))]
[RequireComponent (typeof (PlanetCorruption))]
[RequireComponent (typeof (PlanetEventsManager))]
public class PlanetCorrupted : Planet {
	private PlanetSpawnerManager planetSpawnerManager;
	private PlanetCorruption planetCorruption;
	private PlanetEventsManager planetEventsManager;

	protected override void initialize(){
		planetSpawnerManager = GetComponent<PlanetSpawnerManager> ();
		planetCorruption = GetComponent<PlanetCorruption> ();
		planetEventsManager = GetComponent<PlanetEventsManager> ();
	}

	public override bool isPlanetCorrupted(){
		return true;
	}

	protected override void virtualActivate(){
		if(planetCorruption.isCorrupted()){
			planetCorruption.activateSpawning();
		}
	}

	protected override void virtualDeactivate(){
		planetSpawnerManager.deactivate();
	}
	
	public PlanetSpawnerManager getPlanetSpawnerManager(){
		return planetSpawnerManager;
	}

	public PlanetCorruption getPlanetCorruption(){
		return planetCorruption;
	}

	public PlanetEventsManager getPlanetEventsManager(){
		return planetEventsManager;
	}
}
