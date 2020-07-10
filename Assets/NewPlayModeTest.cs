using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewPlayModeTest {

	[Test]
	public void NewPlayModeTestSimplePasses() {
		// Use the Assert class to test conditions.
		Debug.Log("NewPlayModeTestSimplePasses");
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator NewPlayModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		Debug.Log("NewPlayModeTestWithEnumeratorPasses");
		Assert.AreEqual(1, 2);
		yield return null;
	}

	[UnityTest]
	public IEnumerator MonoBehaviourTest_Works()
	{
		Debug.Log("MonoBehaviourTest_Works");
		yield return new MonoBehaviourTest<MyMonoBehaviourTest>();
	}

	public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
	{
		private int frameCount;
		public bool IsTestFinished
		{
			get { return frameCount > 300; }
		}

		void Update()
		{
			Debug.Log(frameCount);
			frameCount++;
		}
	}
}
