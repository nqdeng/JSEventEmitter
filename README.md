**This library is mainly used in Alibaba.com F2E Team, but it should work for anyone else.**

JsEventEmitter
==============

*The Custom Event library for JScript.NET.*

Introduction
------------

Some dev-tools used in Alibaba.com F2E Team are written with JScript.NET. Because there's no build-in custom event support in JScript.NET so we made one.

Requirement
-----------

To compile and use this library, you need [Microsoft .NET Framework 2.0.](http://www.microsoft.com/download/en/details.aspx?id=19)

Usage
-----

Write a class extending from JsEventEmitter, then instances of this class could emit events, event handlers could also register on instances. See the demo below:

	import Alibaba.F2E.Util;
	import System;

	class Dice extends JsEventEmitter
	{
		public function Throw()
		{
			var num = Math.floor(Math.random() * 6) + 1;
			this.Emit('stop', num);
		}
	}

	class App
	{
		public function App()
		{
			var dice = new Dice();
			dice.AddHandler('stop', OnDiceStop);
			dice.Throw();
		}

		private function OnDiceStop(sender:Object, args:JsEventArgs):void
		{
			var num = Number(args.Data);
			Console.WriteLine('Dice stopped with number {0}', num);
		}
	}

	new App();

APIs
----

###AddHandler(type:String, fn:JsEventHandler):void

Add handler for a specific event.

###RemoveHandler(type:String, fn:JsEventHandler):void

Remove handler of a specific event type.

###RemoveAllHandlers(type:String):void

Remove all handlers of a specific event type.

###Emit(type:String, data:Object):void

Emit a special event with custom data object.

License
-------
JsEventEmitter is released under the MIT license:

>Copyright (c) 2012 Alibaba.Com, Inc.
>
>Permission is hereby granted, free of charge, to any person obtaining a copy of
>this software and associated documentation files (the "Software"), to deal in
>the Software without restriction, including without limitation the rights to
>use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
>of the Software, and to permit persons to whom the Software is furnished to do
>so, subject to the following conditions:
>
>The above copyright notice and this permission notice shall be included in all
>copies or substantial portions of the Software.
>
>THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
>IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
>FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
>AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
>LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
>OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
>SOFTWARE.