/**
 * JS Event Emitter Test Case
 * Copyright (c) 2012 Alibaba.Com, Inc.
 * MIT Licensed
 * @author Nanqiao Deng
 */

import Alibaba.F2E.Util;
import System;

class Pet extends JsEventEmitter {
	var name:String;

	function Pet(name:String) {
		this.name = name;
	}

	function Hungry() {
		Console.WriteLine('Pet.{0}: I am hungry.', this.name);
		this.Emit('hungry', { level: 'very' });
	}

	function Eat(sender:Object, e:JsEventArgs):void {
		Console.WriteLine('Pet.{0}: I ate a {1} from Master.{2}.', this.name, e.Data, sender.name);
	}
}

class Master extends JsEventEmitter {
	var name:String;

	function Master(name:String) {
		this.name = name;
	}

	function Feed(sender:Object, e:JsEventArgs):void {
		Console.WriteLine('Master.{0}: Pet.{1} is {2} {3} and I fed her a fish.', this.name, sender.name, e.Data.level, e.Type);
		this.Emit('feed', 'fish');
	}
}

(function Main() {
	var pet = new Pet('A'),
		masterX = new Master('X'),
		masterY = new Master('Y'),
		masterZ = new Master('Z');

	pet.AddHandler('hungry', masterX.Feed);
	pet.AddHandler('hungry', masterY.Feed);
	pet.AddHandler('hungry', masterZ.Feed);
	masterX.AddHandler('feed', pet.Eat);
	masterY.AddHandler('feed', pet.Eat);
	masterZ.AddHandler('feed', pet.Eat);

	Console.WriteLine('\n### Three masters should feed the ped.\n');
	pet.Hungry();

	Console.WriteLine('\n### Two masters should feed the ped.\n');
	pet.RemoveHandler('hungry', masterX.Feed);
	pet.Hungry();

	Console.WriteLine('\n### No master should feed the ped.\n');
	pet.RemoveAllHandlers('hungry');
	pet.Hungry();
}());