﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostbit.Tweaker.Core
{
	/// <summary>
	/// All invokable tweaker objects implement this interface.
	/// </summary>
	public interface IInvokable : ITweakerObject
	{
		/// <summary>
		/// Get Info about this invokable such as description strings.
		/// </summary>
		InvokableInfo InvokableInfo { get; }

		/// <summary>
		/// Invoke the invokable object with the provided arguments.
		/// Throws InvokeException, InvokeArgNumberException, InvokeArgTypeException
		/// </summary>
		/// <param name="args">Arguments to invoke with. Pass null for no arguments.</param>
		/// <returns>The return value of the invokable.</returns>
		object Invoke(object[] args = null);

		/// <summary>
		/// The manager that this invokable is registered to.
		/// An invokable can only be registered to one manager at any given time.
		/// </summary>
		IInvokableManager Manager { get; set; }
	}
}
