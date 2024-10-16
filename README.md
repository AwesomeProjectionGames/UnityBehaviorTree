# Behavior Tree for Unity
Simple and flexible, open source behavior tree implementation for Unity with a debug editor

![image info](./Img~/bt.png)

## Features
- **Behavior Tree Editor**: A visual editor that allows you to create and edit behavior trees in Unity.
- **Behavior Tree Debugger**: The visual editor also allows you to debug your behavior trees per instance in real-time.
- **Basic Behavior Tree Nodes**: A set of basic behavior tree nodes that can be used to create complex behaviors (like sequence, selector, repeat until, invertor, aborter, wait, etc.).

## Installation
To install this package, you can use the Unity Package Manager. To do so, open the Unity Package Manager and click on the `+` button in the top left corner. Then select `Add package from git URL...` and enter the following URL:

```
https://github.com/AwesomeProjectionGames/UnityBehaviorTree.git
```

Or you can manually to add the following line to your `manifest.json` file located in your project's `Packages` directory.

```json
{
  "dependencies": {
    ...
    "com.awesomeprojection.behaviortree": "https://github.com/AwesomeProjectionGames/UnityBehaviorTree.git"
    ...
   }
}
```

## Usage
### Main Concepts
There are 3 main objects that you will need to use a behavior tree:
- **Behavior Tree Runner**: The runner is a component that you attach to a GameObject to run a behavior tree. You can open the editor by clicking on the `Open Behavior Tree` button to compose your behavior tree.
- **Nodes**: Nodes are the building blocks of a behavior tree. They are the elements that will be executed by the runner.
- **Blackboard**: The blackboard is a data structure that allows you to store and share data between nodes. All nodes have access to the blackboard once the runner has awaked.
#### Abstract Nodes
These are the base nodes types that you can use to create your behavior tree.
- **Composite**: A composite node is a node that can have **multiple children**. To add children to a composite node, you can right-click on the node and select `Add Child`.
- **Pass Through**: A pass-through node is a node that has **only one child**.
- **Conditional**: A conditional node is a node that has **no children** and immediately returns a result based on a condition.
- **Action**: An action node is a node that has **no children** and performs an action over a series of frames.
- **Root**: The root node is the entry point of the behavior tree. It is a pass-through node that has **only one child**. It is the only node that has no parent.
#### Basic Nodes
Here are the nodes that are already available by default :
- **Sequence**: A sequence node is a composite node that executes its children in order (or tick a checkbox to randomize the order before execution). If one of its children fails, the sequence node will return a failure.
- **Selector**: Like the sequence but if one of its children succeeds, the selector node will immediately return a success without executing the other children.
- **Repeat**: A pass-through node that will repeat its child indefinitely or until a condition is met (like until a child returns a failureo or a success).
- **Invertor**: A pass-through node that will invert the result of its child.
- **Aborter**: A pass-through node that will abort the execution if a specific condition child returns a success. This node has 2 children (one for an action and one for a condition).
- **Wait**: A pass-through node that will wait for a specific amount of time before returning a success.
### Making Custom Nodes
Here's an example of how to implement an action node :
```csharp
public class Wait : Action
{
    public float TimeToWait; //This is a public field, it will be exposed in the behavior tree editor
    private float _time;

    protected override void OnRun() //Called each time the node start to be executed
    {
        _time = 0;
    }

    protected override FrameResult OnUpdate() //Called each frame (this is the only method required to implement)
    {
        _time += Time.deltaTime;
        if (_time >= TimeToWait) return FrameResult.Success;
        return FrameResult.Running;
    }
    
    protected override void OnAwake(){} //Called once, when the runner awakes
    protected override void OnAbort(){} //Called when the node is aborted (for example, by aborter)
}
```
Some abstract nodes require a method other than OnUpdate, such as conditional :
```csharp
public class True : Conditional
{
    protected override bool CheckCondition()
    {
        return true;
    }
    //You can also override OnAwake, OnAbort, OnRun methods as well
}
```
### Blackboard
Some scripts will need to access or transmit information. This can be done through the blackboard. Here is an example of how to use it :
```csharp
...
protected override void OnRun()
{
    string gameObjectName = Blackboard.Runner.gameObject.name;
    Debug.Log($"The name of the Runner's GameObject is {gameObjectName}");
}
...
```
### Custom Blackboard
By default, the blackboard only contains a reference to the runner. You could GetComponent OnAwake to access other scripts, but it's the preferred method.
Instead, you can add custom data to the blackboard by creating a custom blackboard class :
```csharp
public class MyCustomBlackboard : Blackboard
{
    public MyCustomBlackboard(BehaviorTreeRunner runner, Transform defaultTarget) : base(runner)
    {
        Target = defaultTarget;
    }
    //This can be accessed by any node and changed at runtime
    public Transform Target;
}
```
Then, you can create a custom runner that uses this blackboard :
```csharp
public class MyCustomBehaviorTreeRunner : BehaviorTreeRunner
{
    [SerializeField] private Transform _defaultTarget;
    protected override Blackboard CreateBlackboard()
    {
        return new MyCustomBlackboard(this, _defaultTarget);
    }
}
```
You can then access the custom blackboard in your nodes :
```csharp
...
protected override void OnRun()
{
    Transform target = (Blackboard as MyCustomBlackboard).Target;
    Debug.Log($"The target is {target.name}");
}
...
```
*Note that the `(Blackboard as MyCustomBlackboard)` should be soon no longer necessary. You will instead be able to use the `Blackboard` property directly and inherit from `Action<T>` or `Conditional<T>` to have access to the custom blackboard.'*