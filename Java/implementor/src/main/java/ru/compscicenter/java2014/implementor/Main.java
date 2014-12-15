package ru.compscicenter.java2014.implementor;

/**
 * Created by анастасия on 27.11.2014.
 */
public class Main {
    public static void main(String[] args) {
        SimpleImplementor imp1 = new SimpleImplementor("Test");
        try {
            String s = imp1.implementFromStandardLibrary("java.net.SocketImpl");
        }
        catch (ImplementorException e){

        }
    }
}
