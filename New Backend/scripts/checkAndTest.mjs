/**
 * @file checkAndTest.mjs
 * @description Smart test runner - only runs tests on fresh database installations
 * Checks if leads collection is empty, runs tests if empty, skips if data exists
 */

import mongoose from 'mongoose';
import { spawn } from 'child_process';
import { dirname } from 'path';
import { fileURLToPath } from 'url';
import dotenv from 'dotenv';

dotenv.config();

const __dirname = dirname(fileURLToPath(import.meta.url));

// Import Lead model
import '../src-v2/modules/leads/models/Lead.mjs';
const Lead = mongoose.model('Lead');

// Get MongoDB connection string from environment
const MONGODB_URI = process.env.MONGODB_URI || 'mongodb://localhost:27017/ccm';

/**
 * Check if database is fresh (no leads collected)
 */
const isDatabaseFresh = async () => {
    let connection = null;
    try {
        console.log('🔍 Checking database state...\n');
        console.log(`   MongoDB URI: ${MONGODB_URI}\n`);

        // Connect to MongoDB
        connection = await mongoose.connect(MONGODB_URI, {
            serverSelectionTimeoutMS: 5000,
            connectTimeoutMS: 5000
        });

        console.log('   ✓ Connected to MongoDB\n');

        // Count documents in leads collection
        const count = await Lead.countDocuments();
        console.log(`   Leads found: ${count}\n`);

        if (count === 0) {
            console.log('✅ Database is fresh (0 leads found)\n');
            console.log('🧪 Running initialization tests...\n');
            return true;
        } else {
            console.log(`⏭️  Database already has ${count} lead(s), skipping tests\n`);
            console.log('💡 Tip: to re-run tests, clear the leads collection\n');
            return false;
        }
    } catch (error) {
        console.log(`❌ Database check error: ${error.message}\n`);

        if (error.name === 'MongoError' || error.message.includes('ECONNREFUSED')) {
            console.log('⚠️  MongoDB not available, skipping tests\n');
            console.log('💡 Run tests manually later with: npm run test:api\n');
            return false;
        }

        console.log('⏭️  Skipping tests due to database error\n');
        console.log('💡 Run tests manually later with: npm run test:api\n');
        return false;
    } finally {
        // Close connection
        if (connection) {
            await mongoose.connection.close();
            console.log('   ✓ Disconnected from MongoDB\n');
        }
    }
};

/**
 * Run the test script as a child process
 */
const runTests = () => {
    return new Promise((resolve) => {
        const testScript = `${__dirname}/runPhase3Tests.mjs`;
        const child = spawn('node', [testScript], {
            stdio: 'inherit',
            cwd: `${__dirname}/..`
        });

        child.on('close', (code) => {
            resolve(code === 0);
        });

        child.on('error', (error) => {
            console.error('Error running tests:', error.message);
            resolve(false);
        });
    });
};

/**
 * Start server for testing
 */
const startServer = () => {
    return new Promise((resolve) => {
        console.log('🚀 Starting server for testing...\n');

        const server = spawn('node', ['index-v2.mjs'], {
            cwd: `${__dirname}/..`,
            stdio: 'pipe'
        });

        let isReady = false;

        server.stdout.on('data', (data) => {
            const output = data.toString();
            if (output.includes('listening on port') || output.includes('Server running')) {
                isReady = true;
                console.log('✓ Server is ready\n');
                resolve(server);
            }
        });

        server.stderr.on('data', (data) => {
            // Log stderr but don't fail
            console.log(`Server: ${data}`);
        });

        // Timeout after 10 seconds
        setTimeout(() => {
            if (!isReady) {
                console.log('⚠️  Server startup timeout, running tests anyway...\n');
                resolve(server);
            }
        }, 10000);
    });
};

/**
 * Stop server after testing
 */
const stopServer = (server) => {
    return new Promise((resolve) => {
        console.log('\n🛑 Stopping test server...\n');
        server.kill('SIGTERM');
        server.on('close', () => {
            resolve();
        });
        setTimeout(() => {
            server.kill('SIGKILL');
            resolve();
        }, 5000);
    });
};

/**
 * Main function
 */
const main = async () => {
    console.log('\n' + '='.repeat(60));
    console.log('BACKEND INSTALLATION CHECKER');
    console.log('='.repeat(60) + '\n');

    const fresh = await isDatabaseFresh();

    if (fresh) {
        let server = null;
        try {
            // Wait 2 seconds for any resources to settle
            await new Promise(resolve => setTimeout(resolve, 2000));

            // Start server for testing
            server = await startServer();

            // Wait for server to fully initialize
            await new Promise(resolve => setTimeout(resolve, 2000));

            // Run tests
            await runTests();
        } catch (error) {
            console.error('Error during testing:', error.message);
        } finally {
            // Stop server if it was started
            if (server) {
                await stopServer(server);
            }
        }
    }

    console.log('\n' + '='.repeat(60));
    console.log('Installation check complete');
    console.log('='.repeat(60) + '\n');

    process.exit(0);
};

// Run
main().catch(error => {
    console.error('Fatal error:', error.message);
    process.exit(1);
});
