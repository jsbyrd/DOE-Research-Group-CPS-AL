using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionList {
    public int count;
    public Instruction head;
    public Instruction tail;

    public InstructionList() {
        this.count = 0;
        this.head = null;
        this.tail = null;
    }

    public void Add(Instruction instruction) {
        this.count++;
        if (head == null) head = instruction;
        if (tail != null) tail.next = instruction;
        tail = instruction;
    }

    public void AddFirst(Instruction instruction) {
        this.count++;
        if (tail == null) tail = instruction;
        instruction.next = head;
        head = instruction;
    }
}