package ru.compscicenter.java2014.collections.test;

import org.testng.annotations.Factory;

import java.util.Locale;
import java.util.Properties;

public class MultiSetTestFactory {

    @Factory
    public static Object[] createTests() throws Exception {
        Properties prop = new Properties();
        prop.load(MultiSetTestFactory.class.getClassLoader().getResourceAsStream("build.properties"));
        Locale.setDefault(Locale.US);
        return new Object[] { new MultiSetTest(Class.forName(prop.getProperty("IMPLEMENTATION_CLASS"))) };
    }

}
