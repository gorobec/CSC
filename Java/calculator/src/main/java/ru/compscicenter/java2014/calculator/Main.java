package ru.compscicenter.java2014.calculator;

import java.io.BufferedReader;
import java.io.InputStreamReader;

public class Main
{

    public static void main(String[] args) throws Exception
    {
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        String input = reader.readLine();

        Calc calculator = new Calc();

        System.out.print(calculator.calculate(input));
    }
}
